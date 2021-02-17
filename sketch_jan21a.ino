#include <SoftwareSerial.h>

#define rxPin 11 // RX pin is connected to port 11 on the arduino card
#define txPin 10 // TX pin is connected to port 10 on the arduino card


const int trigPin = 2; //trigger pin is connected to port 2 on the arduino card
const int echoPin = 3; //echo pin is connected to port 3 on the arduino card
const int irPin = 5; //ir pin is connected to port 5 on the arduino card
const int led1Pin = 6; //led 1 pin is connected to port 6 on the arduino card
const int led2Pin = 7; //led 2 pin is connected to port 7 on the arduino card
unsigned long lastShot = 0; //initialize the long for fire sensor detection
unsigned long waitTime = 2000; //set the wait time to 3s
const double scoreThreshold = 7; //set the detection distance to 7cm
int isUltS = 0;

SoftwareSerial mySerial(rxPin, txPin);

void setup()
{
 // define pin modes for echo sensor
 pinMode(rxPin, INPUT);
 pinMode(txPin, OUTPUT);
 pinMode(irPin,INPUT);

 //defin pin modes for led
 pinMode(led1Pin, OUTPUT);
 pinMode(led2Pin, OUTPUT);

 //define pin modes for laser sensor
 pinMode(trigPin, OUTPUT);
 pinMode(echoPin, INPUT);
 
 //define serials for arduino card
 mySerial.begin(9600);
 Serial.begin(9600);
}

void loop()
{
  if(isUltS == 0)
  {
    if(detectScore() && millis()-lastShot >= waitTime)
    {
      Serial.println("detect ult s");
      isUltS = 1;
      lastShot = millis();
    }
  }
  if(digitalRead(irPin) == 0 && isUltS == 1)
    {
      mySerial.println("score +1");
      Serial.println("score +1");
      scoredLed();
      isUltS = 0;
    }
}

//@return true if shot is detected, false if otherwise
//Detects whether or not a shot was made by checking if distance < "threshold."
boolean detectScore()
{
  return (distance() <= scoreThreshold);
}

void scoredLed()
{
  digitalWrite(led1Pin, HIGH);
  digitalWrite(led2Pin, HIGH);
  delay(5000);
  digitalWrite(led1Pin, LOW);
  digitalWrite(led2Pin, LOW);
}

//double distance()
//@return the distance in centimeters (cm)
//Calculates the distance from the sensor to the next closest object.
double distance()
{
  double duration = 0;
  digitalWrite(trigPin, HIGH); //send out pulse
  delayMicroseconds(50); //give the pulse time
  digitalWrite(trigPin, LOW); //turn off pulse
  duration = pulseIn(echoPin, HIGH); //read echo pin
  return (duration/2) / 29.1; //in cm
}
