#include <ESP8266HTTPClient.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiAP.h>
#include <ESP8266WiFiGeneric.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266WiFiScan.h>
#include <ESP8266WiFiSTA.h>
#include <ESP8266WiFiType.h>
#include <WiFiClient.h>
#include <WiFiClientSecure.h>
#include <WiFiServer.h>
#include <WiFiUdp.h>
#include <Adafruit_NeoPixel.h>

#define UUID          "{[UUID]}"   //dynamic setting
#define MIN_VERSION   {[MIN]}
#define MAJ_VERSION   {[MAJ]}
#define DEVICE_NAME   "{[DEVICE]}"
#define FIRMWARE_ID   "{[FW_ID]}"

#define WIFI_SSID     "{[WIFI_SSID]}"      //dynamic setting
#define WIFI_PWD      "{[WIFI_PWD]}"           //dynamic setting
//#define PORT          4001                  //dynamic setting
#define TARGET_URL    "{[TARGET_URL]}"           //dynamic setting
#define TIMEOUT       {[TIMEOUT]}                  //dynamic setting


#define PIN           4
#define NUMPIXELS     {[LED_COUNT]}                   //dynamic setting


//commands for TCP Interface
#define CMD_QUIT      "_QUIT"               //command for closing the connection
#define CMD_CLEAR     "CLEAR"               //command for clearing all pixels
#define CMD_SHOW      "_SHOW"               //command for showing pixels
#define CMD_NOPE      "_NOPE"               //command for no further commands
#define CMD_SET       "__SET"               //command for setting pixels
#define CMD_HELLO     "HELLO"               //command for initial Hello message  


Adafruit_NeoPixel pixels = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
//WiFiServer server(PORT);
//WiFiClient client;

void setup() {
  Serial.begin(9600);
  setupPixels();
  setupWiFi(1);
}

void setupPixels(){
  pixels.begin();
  pixels.show();
}

void setupWiFi(int withFinalAnimation){
  WiFi.begin(WIFI_SSID,WIFI_PWD);
  int counter = 0;

  Serial.println("Try to connect");
  
  //while not connected...
  while(WiFi.status() != WL_CONNECTED){
    //clear all pixels
    pixels.clear();
    switch(WiFi.status()){
      case WL_CONNECT_FAILED: //if the password is incorrect
      case WL_NO_SSID_AVAIL: //if the WiFi Network is not available
        setPixel(counter,255,0,0);
        setPixel(counter-1,255,0,0);
        setPixel(counter-2,255,0,0);
        Serial.print("<SSID_O_PWD_FAIL>");
        break;
      case WL_DISCONNECTED: //if still disconnected
      case WL_IDLE_STATUS:
        setPixel(counter,127,127,0);
        setPixel(counter-1,127,127,0);
        setPixel(counter-2,127,127,0);
        Serial.print(".");
        break;
    }
    //show
    pixels.show();

    //increase counter
    counter++;
    if(counter == NUMPIXELS){
      counter = 0;
    }
    //delay
    delay(20);
  }

  Serial.println("connected");
  
  //when connected
  int counter2 = 0;
  //show animation one last time
  while(counter2 < NUMPIXELS && withFinalAnimation == 1){
      pixels.clear();
      setPixel(counter,0,255,0);
      setPixel(counter-1,0,255,0);
      setPixel(counter-2,0,255,0);
      pixels.show();
      delay(20);
      counter++;
      counter2++;
      if(counter == NUMPIXELS){
        counter = 0;
      }
  }
  pixels.clear();
  pixels.show(); 

  Serial.print("IP Address: ");
  Serial.println(WiFi.localIP());
  
  //start Server
  //server.begin();

}

void checkWiFi(){
  if(WiFi.status() != WL_CONNECTED){
    setupWiFi(0);
  }
}

int setPixel(int pixel, int red, int green, int blue){
  if(pixel >=0 && pixel < NUMPIXELS){
    //set pixel if the number is in range
    pixels.setPixelColor(pixel, pixels.Color(red,green,blue));
  }
}


void loop() {
  //check WiFi first
  checkWiFi();

  HTTPClient client;
  WiFiClient wClient;
  
  client.begin(TARGET_URL);
  client.addHeader("Content-Type","text/plain");
  client.addHeader("Authorization",UUID);
  Serial.println("Send HTTP Request");
  int statusCode = client.POST(String(CMD_HELLO)+"#"+UUID+"#"+DEVICE_NAME+"#"+FIRMWARE_ID+"#"+MAJ_VERSION+"#"+MIN_VERSION);
  Serial.println(String("Status Code: ")+statusCode);
  if(statusCode == 200){
      int len = client.getSize();
      int cLen = len; //copy of len (we will need this value later)
      Serial.println(String("Length of Response-Payload is: ")+len);
      if(len > 0){ //length header must be set and value greater than 0
         //get stream
         WiFiClient* stream = client.getStreamPtr();
         //prepare message buffer (same size as length)
         uint8_t complete[1024] = {0};
         //prepare buffer 
         uint8_t buffer[128] = {0};
         //as long as there is a connection and there are still symbols left
         while(client.connected() && len > 0){
             //size contains the number of symbols which are ready for reading
             size_t sizeStream = stream->available(); 
             if(sizeStream){
                int c = stream->readBytes(buffer, ((sizeStream>sizeof(buffer))?sizeof(buffer):sizeStream)); //read stream and write available symbols into the buffer (but do not exceed the maximal size of the buffer) 
                Serial.println(String("Length of loaded content: ")+c);

                //copy to complete buffer
                for(int i = 0; i < c; i++){
                  complete[cLen-len+i] = buffer[i];
                }
                //decrease the toal length
                len -= c;                
             }
             delay(1); //wait
         }
         //yield();
         
        for(int i = 0; i < cLen; i=i+4){
            if(i+3 < cLen){
                int pixel = complete[i];
                int red = complete[i+1];
                int green = complete[i+2];
                int blue = complete[i+3];
                //Serial.println(String("Set Pixel ")+pixel+" with "+red+":"+green+":"+blue);
                pixels.setPixelColor(pixel, pixels.Color(red,green,blue));
                if(i < 4){
                  Serial.println(String("Color of pixel:")+pixel+", RED: "+red+", GREEN:  "+green+", BLUE: "+blue);  
                }
            }
            else
            {
                Serial.println(String("Invalid Pixel at ")+i);  
            }
        }
        pixels.show();
        Serial.println("completed");
        
      }
      else
      {
        Serial.println("Response-Payload is empty");  
      }
  }
  client.end();
  delay(1000); 
}