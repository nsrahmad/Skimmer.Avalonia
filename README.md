Skimmer is a simple RSS/Atom Feed Reader built with modern .Net, leveraging Native AOT Compilation.

<img width="1042" height="578" alt="Screenshot 2025-09-12 161024" src="https://github.com/user-attachments/assets/27518f43-d76b-4118-a2c4-8f52036cfb55" />

It supports Dark/Light Mode.

<img width="1049" height="586" alt="Skimmer-themes" src="https://github.com/user-attachments/assets/cd1182cb-d2ec-45f9-93d4-1796cdaa87b3" />

On Linux (AFAIK due to Wayland restrictions) the combined header with window decoration is handled differently:

<img width="1064" height="246" alt="Screenshot_20250912_154612-1" src="https://github.com/user-attachments/assets/c55bb489-fe0c-4ff4-97c1-4fd0931214dc" />

### Benefits of Native AOT Compilation
 - The Installer of Skimmer is around ~15 MB, there is no other separate Runtime installation needed. It is completely self-contained.
 - Skimmer starts up really fast, like a native C/C++ program.

### Building
See the parent [Skimmer](https://github.com/nsrahmad/Skimmer) which contains the [Skimmer.Core](https://github.com/r/nsrhamad/Skimmer.Core)
and this repository in a single Solution. Just checkout and build in any .Net IDE.


