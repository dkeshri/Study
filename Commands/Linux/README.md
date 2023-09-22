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


## To disable any key of keyboard use below cmd

to find keycode use below cmd and press that key you want to disable
```bash
xev
```

then to disable use keycode to disable that key for Ex my pgDown key is causing problem and keycode is `117`

then use below command any will work

```bash
xmodmap -e 'keycode 117 = NoSymbol'
```
or 
```bash
xmodmap -e 'keycode 117 = '
```
or

```bash
xmodmap -e 'keycode 117 = 0x0000'
```
to check changed is applied or not use below cmd that will show the value associated with key

```bash
xmodmap -pke
```

