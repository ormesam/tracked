# MTB Mate

An attempt to track stuff such as speed and jumps on a mountain bike using an android phone.

## Notes/Train of Thoughts
- Attached phone to bike and went down jump track, results weren't great as the phone was big and the holder moved about too much. Maybe attach the phone in a better place? Or attach an accelerometer directly to the bike and send the data to the phone?

- Planning to use an arduino/raspberry pi zero to record the accelerometer data and stream it to the app via bluetooth. Arduino seems easier to implement however the RaspberryPi has built in bluetooth.

- Used Arduino with MPU-6050 and hc-06 bluetooth module to send accelerometer data to the phone via bluetooth, works well but not tested on the track yet.

- The Arduino Nano 33 IOT has built in bluetooth and accelerometer/gyro: https://store.arduino.cc/nano-33-iot. Ordered, we will see how it goes...
