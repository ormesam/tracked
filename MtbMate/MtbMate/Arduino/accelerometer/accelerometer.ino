// Tested on Arduino Nano 33 IOT

#include <ArduinoBLE.h>
#include <Arduino.h>
#include "LSM6DS3.h"
#include "Wire.h"

BLEService accService("19B10001-E8F2-537E-4F6C-D104768A1214");
BLEStringCharacteristic accCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214", BLERead | BLENotify, 15);

const int ledPin = LED_BUILTIN;

void setup() {
  Serial.begin(9600);
  Wire.begin();
  accelerometer.begin();
  
  if(accelerometer.isActive()){
    Serial.println("Accelerometer already active");
  }else{
    if(accelerometer.powerOn()){
      Serial.println("Accelerometer Power ON");
    }else{
      Serial.println("Accelerometer Not Powered On");
    }
  }

  // set LED pin to output mode
  pinMode(ledPin, OUTPUT);

  // begin initialization
  if (!BLE.begin()) {
    Serial.println("starting BLE failed!");

    while (1);
  }

  // set advertised local name and service UUID:
  BLE.setLocalName("Mtb Mate");
  BLE.setAdvertisedService(accService);

  // add the characteristic to the service
  accService.addCharacteristic(accCharacteristic);

  // add service
  BLE.addService(accService);

  // set the initial value for the characeristic:
  accCharacteristic.writeValue("");

  // start advertising
  BLE.advertise();

  Serial.println("Starting Mtb Mate");
}

void loop() {
  // listen for BLE peripherals to connect:
  BLEDevice central = BLE.central();

  // if a central is connected to peripheral:
  if (central) {
    Serial.print("Connected to device: ");
    // print the central's MAC address:
    Serial.println(central.address());

    // while the central is still connected to peripheral:
    while (central.connected()) {
          float x = accelerometer.getConvertedXAxis();
          float y = accelerometer.getConvertedXAxis();
          float z = accelerometer.getConvertedXAxis();
      
          Serial.print("X = ");
          Serial.print(x, 2);
          Serial.print("g  Y = ");
          Serial.print(y, 2);
          Serial.print("g  Z = ");
          Serial.print(z, 2);
          Serial.println("g");

          String buf;
            buf += String(roundf(x*100.0)/100.0);
            buf += F(",");
            buf += String(roundf(y*100.0)/100.0);
            buf += F(",");
            buf += String(roundf(z*100.0)/100.0);
          
          accCharacteristic.setValue(buf);

          delay(250);
    }

    // when the central disconnects, print it out:
    Serial.print(F("Disconnected from device: "));
    Serial.println(central.address());
  }
}
