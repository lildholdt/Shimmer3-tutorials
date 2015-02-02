ShimmerConnect allows users to display and save data received from Shimmer devices streaming over Bluetooth. The application is designed for greater usability and functionality, with a number of data capture parameters being configurable.

Users can select the sampling rate, which sensors are to be used, enable/disable power monitoring, and change parameters such as the accelerometer’s range. Once captured, the data can then be saved to a CSV file for further interpretation and analysis.

This application runs natively in Windows and can be run in Linux using mono (http://www.mono-project.com/Main_Page). See README.LINUX for information on compiling application to run in Linux. Set .Net Framework 2 when using on Mono.

Compilation tested in Microsoft Visual Studio 2008 and 2010, and Mono 2.6.7 and 2.8. When compiling for Windows 7 (and probably Vista, but not tested) Microsoft Visual Studio 2010 should be used to get correct detection of serial ports


This application communicates with a shimmer running the BoilerPlate TinyOS application, which is available from Sourceforge CVS at http://tinyos.cvs.sourceforge.net/viewvc/tinyos/tinyos-2.x-contrib/shimmer/apps/BoilerPlate/



Changelog:
- V0.1 (10 Jan 2011)
   - initial release
   - support for Accelerometer, Gyroscope, Magnetometer, ECG, EMG, AnEx sensors
   - Sampling rate, Accel range, 5V regulater and PMUX (volatage monitoring) configurable
   - save data to .csv file
- V0.2 18 May 2011
   - support for Strain Gauge and Heart Rate sensors
   - fixed problem with saving/displaying Magnetometer data
      - read Mag data as 16-bit signed int, instead of 32-bit/64-bit
   - added MagHeading box when magnetometer is enabled
   - fixed EMG problem
      - a second EMG channel was being added erroneously
   - support for receiving 8-bit data channels instead of just 16-bit
   - "AnEx ADC0" and "AnEx ADC7" labels in Configure window now change to "VSenseReg" and "VSenseBatt" respectively when voltage monitoring is enabled
   - GSR support
- V0.3 15 April 2013
   - now supports the BT Stream firmware, among the key features of BT Stream is that there is now a low battery indicator when the VSenseBatt is enabled and the voltage drops below 3.4V.For further details please refer to the user guide
   - calibrated data is now logged
- V0.4 24 April 2013
   - now supports 3D orientation in quartenions. 3D orientation algorithm can be found here : Madgwick, Sebastian OH, Andrew JL Harrison, and Ravi Vaidyanathan. "Estimation of imu and marg orientation using a gradient descent algorithm." Rehabilitation Robotics (ICORR), 2011 IEEE International Conference on. IEEE, 2011.
   - 3D Cube example included. This example require the tao framework. This example has not been tested on Linux
- v0.5 30 Sept 2013
   - now supports Shimmer3
- v0.6 15 Oct 2013
   - does not support Shimmer3 pressure sensor	