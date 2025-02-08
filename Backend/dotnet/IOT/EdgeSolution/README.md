# Azure IOT Edge 
For the development we will use iotedgehubdev simulator.

## Setup Local Development : VS code

**Step 1** : Install Python(3.10.9) with PIP.

1. [Python 3.10.9](https://www.python.org/downloads/release/python-3109/) After Installer download Run that Installer and Please Select **Custom Installation** and then select PIP in the Checkbox as below image.

    ![Python with pip](./imgs/PytPip.png)

    After Installation Check Version in Cmd
    ```bash
    py --version
    ```
    ![Python version](./imgs/pyversion.png)


2. Install [IOTEDGEHUBDEV](https://pypi.org/project/iotedgehubdev/0.10.0/)

    ```bash
    pip install --upgrade iotedgehubdev
    ```
    
    check in environment variable Python Script path should be there if not then please add as below image

    `C:\Users\YourPcUsername\AppData\Local\Programs\Python\Python310\Scripts\`
    ![PyScriptEnvironmentVariable](./imgs/PyEnv.png)

    Setup IoTEdgehubdev

    * Window

        ```bash
        iotedgehubdev setup -c "<edge-device-connection-string>"
        ```
    * Linux/macOS
        ```bash
        sudo iotedgehubdev setup -c "<edge-device-connection-string>"
        ```
    Or We can also setup in Vs code too see below steps in VS code Extension step.


**Step 2** : Install Azure IoT Edge Extension

![Azure IoT Edge Extension](./imgs/azure_iot_edge.png)

