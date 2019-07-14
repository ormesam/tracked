## Notes/Train of Thoughts
- Attached phone to bike and went down jump track, results weren't great as the phone was big and the holder moved about too much. Maybe attach the phone in a better place? Or attach an accelerometer directly to the bike and send the data to the phone?

- Planning to use an arduino/raspberry pi zero to record the accelerometer data and stream it to the app via bluetooth. Arduino seems easier to implement however the RaspberryPi has built in bluetooth.

- Used Arduino Uno with MPU-6050 and HC-06 Bluetooth module to send accelerometer data to the phone via Bluetooth, works well but not tested on the track yet.

- The Arduino Nano 33 IOT has built in Bluetooth and accelerometer/gyro: https://store.arduino.cc/nano-33-iot. Ordered, and tested. Works very well and is much smaller than the Arduino Uno and boards, also we can just upload the code straight to it and it works, no need for plugging additional boards in etc.

- Keeping the work done for the phone accelerometer for now.
