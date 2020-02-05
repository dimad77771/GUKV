Satellite assemblies and a test application are in the DevExpress.DLL subfolder.
The test application can be launched with a command line argument specifying the UI culture.
For example:

TestLocalization.exe da-DK

Here "da-DK" denotes the Danish (Denmark) UI culture. It will force loading "da" satellite 
assemblies into the test application.

Available translations
----------------------
ca	Catalan
cs	Czech
da	Danish
de	German
el	Greek
es	Spanish
fi-FI	Finnish
fr	French
hr	Croatian
is-IS	Icelandic
it	Italian
ja	Japanese
nl	Dutch
no	Norwegian
pl	Polish
pt	Portuguese
pt-BR	Portuguese (Brazil)
ro	Romanian
ru	Russian
sl	Slovenian
sv	Swedish
vi	Vietnamese
zh-CHS	Chinese (Simplified)
zh-CN	Chinese (China)
zh-TW	Chinese (Taiwain)


If You Decide to Recompile Satellite Assemblies Yourself
--------------------------------------------------------
Satellite assemblies must be signed with the same strong key token as the main 
DevExpress.~ assemblies.  Therefore, you will need to rebuild our assemblies as well 
if you want to compile resource files yourself.  You can do this only if you have
full source code of all .NET components (if you own DXperience Enterprise or Universal).

1. Use the build scripts from article A609 to rebuild main assemblies:
http://www.devexpress.com/Support/Center/KB/p/A609.aspx
2. Copy StrongKey.snk from 
C:\Program Files\DevExpress 2010.2\Components\Sources\DevExpress.Key\
to DevExpress.Key subfolder in the current directory.
3. Open Localization.sln in Visual Studio and rebuild it.
Satellite assemblies are output into the DevExpress.DLL subfolder.