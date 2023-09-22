# Linux Command in Terminal

## Basic command

Check for update the system 
```bash
sudo apt update
```
Now in acutal update the system application.
```bash
sudo apt upgrade
```
we can run both command at same time
```bash
sudo apt update && sudo apt upgrade
```

## Install Application
install the application 
```bash
sudo apt install <PackageName or PackageNames(space saperated)>
```
example to install VLC write the below command 
```bash
sudo apt install vlc
```
To install multiple application write the command like below example.

>like we are installing **Gpated,VLC and pdfsam-basic** in single command

```bash
sudo apt install gparted vlc pdfsam-basic
```

## Uninstall Application

### remove the installed Application

```bash
sudo apt remove <PackageName>
```

>remove everything of that Package use **purge**

```bash
sudo apt-get --purge remove <PackageName>
```
**Example** => Remove conky application
```bash
sudo apt-get --purge remove conky conky-all
```