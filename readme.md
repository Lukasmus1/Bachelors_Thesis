# Kernel_Panic

> *Some systems were never meant to be booted up. A fourth-wall-breaking meta-horror experience.*

![Platform](https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011-blue)
![Engine](https://img.shields.io/badge/Made%20with-Unity-black)
![License: CC0](https://img.shields.io/badge/License-CC0%201.0-lightgrey.svg)

**Kernel_Panic** is an experimental meta-horror video game that blurs the lines between a simulated digital environment and the player's actual operating system.

This project was developed as the practical part of a Bachelor's thesis at the **Brno University of Technology, Faculty of Information Technology (VUT FIT)**.

---

## ⚠️ Important Disclaimer

**This game is completely safe and does NOT contain malware.**
However, to deliver its core meta-horror experience, *Kernel_Panic* interacts with your actual Windows OS.

* It will programmatically create and read specific text files on your physical Desktop.
* It utilizes native Windows dialog boxes outside the main game window.
* It will **never** read, modify, or delete your personal files.
* *Note: Windows Defender or other antivirus software might flag the executable as an "unrecognized app" due to these interactions. This is a false positive.*

---

## 🧩 Key Features & Mechanics

* **Simulated OS:** A fully functional desktop environment built within Unity, featuring window layering, a taskbar, and integrated applications (Chat Terminal, File Manager, Settings).
* **Fourth-Wall Breaks:** Deliberate subversion of user control, including fake system crashes, forced desktop interactions, and window manipulation.
* **Cryptographic Algorithms:** Features actual cryptographic implementations as core puzzle mechanics, including:
  * **XOR Cipher** (Stream cipher utilizing PRNG)
  * **Vigenère Cipher**
  * **SHA-256 Hashing** (For external ARG validation)
* **Autostereograms:** Procedurally or statically generated 3D illusions hiding critical visual clues.
* **Web ARG:** Gameplay extends beyond the executable to a custom external website.

---

## 🚀 Installation & Playing

### For Players

1. Download the latest release from [itch.io](https://lukasmus.itch.io/kernel-panic).
2. Extract the `.zip` archive.
3. Run `Kernel_Panic.exe`.

### For Developers (Building from Source)

1. Clone the repository:

   ```bash
   git clone [https://github.com/Lukasmus1/Bachelors_Thesis.git](https://github.com/Lukasmus1/Bachelors_Thesis.git)