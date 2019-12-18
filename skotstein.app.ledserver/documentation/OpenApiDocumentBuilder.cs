// MIT License
//
// Copyright (c) 2019 Sebastian Kotstein
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration;
using Microsoft.OpenApi.CSharpAnnotations.DocumentGeneration.Models;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace skotstein.app.ledserver.documentation
{
    /// <summary>
    /// The class <see cref="OpenApiDocumentBuilder"/> implements methods for creating an Open API documentation (formally 
    /// </summary>
    public class OpenApiDocumentBuilder
    {
        public const string XDOCUMENT_PATH = @".\skotstein.app.ledserver.xml";
        public const string ASSEMBLY_PATH = @".\skotstein.app.ledserver.exe";

        public const string OPEN_API_DOC = @".\OpenApi.json";

        public OpenApiDocument Build(string documentVersion)
        {
            OpenApiGeneratorConfig input = new OpenApiGeneratorConfig(
                annotationXmlDocuments: new List<XDocument>()
                {
                    XDocument.Load(XDOCUMENT_PATH)
                },
                assemblyPaths: new List<string>()
                {
                    ASSEMBLY_PATH
                },
                openApiDocumentVersion: documentVersion,
                filterSetVersion: FilterSetVersion.V1
            );

            GenerationDiagnostic result;
            OpenApiGenerator generator = new OpenApiGenerator();
            IDictionary<DocumentVariantInfo, OpenApiDocument> documents = generator.GenerateDocuments(input, out result);

            foreach(OperationGenerationDiagnostic diagnostic in result.OperationGenerationDiagnostics)
            {
                foreach(GenerationError error in diagnostic.Errors)
                {
                    Console.Write("OpenAPI Document Generation Error in " + diagnostic.OperationMethod + " " + diagnostic.Path);
                    Console.Error.WriteLine(error.Message);
                }
            }

            return documents[DocumentVariantInfo.Default];
        }

        public string AsJson(OpenApiDocument document)
        {
            StringWriter writer = new StringWriter();
            OpenApiJsonWriter jsonWriter = new OpenApiJsonWriter(writer);
            document.SerializeAsV3(jsonWriter);
            return writer.ToString();
        }

        public string AsYaml(OpenApiDocument document)
        {
            StringWriter writer = new StringWriter();
            OpenApiYamlWriter yamlWriter = new OpenApiYamlWriter(writer);
            document.SerializeAsV3(yamlWriter);
            return writer.ToString();
        }

        public string Replace(string documentInput, object values)
        {
            Type type = values.GetType();

            foreach(PropertyInfo property in type.GetProperties())
            {
                foreach(Attribute attribute in property.GetCustomAttributes())
                {
                    if(attribute is Replaces && property.PropertyType == typeof(string))
                    {
                        documentInput = documentInput.Replace("\""+((Replaces)attribute).Placeholder+"\"", (string)property.GetValue(values, null));
                    }
                }
            }

            return documentInput;
        }



        public OpenApiDocument AddSecuritySchema(OpenApiDocument document, IList<HttpController> controllers, /*string authorizationUrl,*/ string tokenUrl)
        {
            string schemaName = "oauth";

            OpenApiSecurityScheme globalScheme = new OpenApiSecurityScheme();
            globalScheme.BearerFormat = "JWT";
            globalScheme.Description = "OAuth 2.0";
            globalScheme.Type = SecuritySchemeType.OAuth2;
            globalScheme.Name = schemaName;
            globalScheme.Scheme = "bearer";
            globalScheme.Flows = new OpenApiOAuthFlows();

            globalScheme.Flows.Password = new OpenApiOAuthFlow()
            {
                //AuthorizationUrl = new Uri(authorizationUrl),
                TokenUrl = new Uri(tokenUrl),

            };
            globalScheme.Flows.ClientCredentials = new OpenApiOAuthFlow()
            {
                //AuthorizationUrl = new Uri(authorizationUrl),
                TokenUrl = new Uri(tokenUrl),
            };

            IList<string> globalScope = new List<string>();

            foreach(HttpController controller in controllers)
            {
                Type type = controller.GetType();
                foreach(MethodInfo method in type.GetMethods()){
                    HttpMethod verb = HttpMethod.UNKNOWN;
                    string path = "";
                    IList<string> localScopes = new List<string>();

                    //step 1: analyze attributes
                    foreach(Attribute attribute in method.GetCustomAttributes())
                    {
                        if(attribute is SKotstein.Net.Http.Attributes.Path)
                        {
                            SKotstein.Net.Http.Attributes.Path p = (SKotstein.Net.Http.Attributes.Path)attribute;
                            verb = p.Method;
                            path = p.Url;
                        }
                        if(attribute is skotstein.net.http.oauth.AuthorizationScope)
                        {
                            skotstein.net.http.oauth.AuthorizationScope s = (skotstein.net.http.oauth.AuthorizationScope)attribute;
                            if (!localScopes.Contains(s.Scope))
                            {
                                localScopes.Add(s.Scope);
                            }
                           
                        }
                    }
                    //step 2: add scopes to operation
                    if (!String.IsNullOrWhiteSpace(path) && document.Paths.ContainsKey(path))
                    {
                        OpenApiPathItem openApiPath = document.Paths[path];
                        OpenApiOperation operation = null;
                        switch (verb)
                        {
                            case HttpMethod.GET:
                                if (openApiPath.Operations.ContainsKey(OperationType.Get))
                                {
                                    operation = openApiPath.Operations[OperationType.Get];
                                }
                                break;
                            case HttpMethod.POST:
                                if (openApiPath.Operations.ContainsKey(OperationType.Post))
                                {
                                    operation = openApiPath.Operations[OperationType.Post];
                                }
                                break;
                            case HttpMethod.PUT:
                                if (openApiPath.Operations.ContainsKey(OperationType.Put))
                                {
                                    operation = openApiPath.Operations[OperationType.Put];
                                }
                                break;
                            case HttpMethod.DELETE:
                                if (openApiPath.Operations.ContainsKey(OperationType.Delete))
                                {
                                    operation = openApiPath.Operations[OperationType.Delete];
                                }
                                break;
                            case HttpMethod.PATCH:
                                if (openApiPath.Operations.ContainsKey(OperationType.Patch))
                                {
                                    operation = openApiPath.Operations[OperationType.Patch];
                                }
                                break;
                        }
                        if(operation != null)
                        {
                            OpenApiSecurityRequirement security = new OpenApiSecurityRequirement();
                            //OpenApiSecurityScheme scheme = new OpenApiSecurityScheme();
                            //scheme.Name = schemaName;
                            //security.Add(globalScheme, localScopes);
                            OpenApiSecurityScheme scheme = new OpenApiSecurityScheme();
                            OpenApiReference reference = new OpenApiReference();
                            reference.Id = schemaName;
                            reference.Type = ReferenceType.SecurityScheme;
                            scheme.Reference = reference;

                            

                            security.Add(scheme, localScopes);
                            operation.Security.Add(security);

                            //step 3: add local scopes to global scopes
                            foreach(string scope in localScopes)
                            {
                                if (!globalScope.Contains(scope))
                                {
                                    globalScope.Add(scope);
                                }
                            }

                            //step 4: add 401 response (Unauthorized)
                            OpenApiResponse response401 = new OpenApiResponse();
                            response401.Description = "The client is unauthorized";
                            OpenApiMediaType mediaType401 = new OpenApiMediaType();
                            OpenApiSchema schema401 = new OpenApiSchema();
                            OpenApiReference reference401 = new OpenApiReference();
                            reference401.Id = "skotstein.app.ledserver.model.ErrorMessage";
                            reference401.Type = ReferenceType.Schema;
                            schema401.Reference = reference401;
                            mediaType401.Schema = schema401;

                            OpenApiExample example401_0 = new OpenApiExample();
                            OpenApiString openApiStringUnauthorized_0 = new OpenApiString("$EXAMPLE_Error_Message_MissingAccessToken");
                            example401_0.Value = openApiStringUnauthorized_0;
                            mediaType401.Examples.Add("Missing access token", example401_0);

                            OpenApiExample example401_1 = new OpenApiExample();
                            OpenApiString openApiStringUnauthorized_1 = new OpenApiString("$EXAMPLE_Error_Message_InvalidAccessToken");
                            example401_1.Value = openApiStringUnauthorized_1;
                            mediaType401.Examples.Add("Invalid access token", example401_1);

                            OpenApiExample example401_2 = new OpenApiExample();
                            OpenApiString openApiStringUnauthorized_2 = new OpenApiString("$EXAMPLE_Error_Message_TokenHasExpired");
                            example401_2.Value = openApiStringUnauthorized_2;
                            mediaType401.Examples.Add("Token has expired", example401_2);

                            response401.Content.Add("application/json", mediaType401);
                            operation.Responses.Add("401", response401);

                            //step 5: add 403 response (Forbidden)
                            OpenApiResponse response403 = new OpenApiResponse();
                            response403.Description = "The client is not allowed to access this resource. Make sure that the following scopes are granted to the client: ";
                            foreach(string scope in localScopes)
                            {
                                response403.Description += scope + " \n";
                            }
                            OpenApiMediaType mediaType403 = new OpenApiMediaType();
                            OpenApiSchema schema403 = new OpenApiSchema();
                            OpenApiReference reference403 = new OpenApiReference();
                            reference403.Id = "skotstein.app.ledserver.model.ErrorMessage";
                            reference403.Type = ReferenceType.Schema;
                            schema403.Reference = reference403;
                            mediaType403.Schema = schema403;
                            OpenApiExample example403 = new OpenApiExample();
                            OpenApiString openApiStringForbidden = new OpenApiString("$EXAMPLE_Error_Message_Forbidden");
                            example403.Value = openApiStringForbidden;
                            mediaType403.Examples.Add("Invalid scope", example403);
                            response403.Content.Add("application/json", mediaType403);
                            operation.Responses.Add("403", response403);    


                        }
                    }
                }

            }
           

            foreach(string scope in globalScope)
            {
                string description = "";
                string[] split = scope.Split('-');
                if(split.Length == 2)
                {
                    description = "Grants " + split[1] + " access to " + split[0] + " resources";
                }

                globalScheme.Flows.Password.Scopes.Add(scope, description);
                globalScheme.Flows.ClientCredentials.Scopes.Add(scope, description);
            }

            document.Components.SecuritySchemes.Add(schemaName, globalScheme);
            return document;
        }
    }
}
