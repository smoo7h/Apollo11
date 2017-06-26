;#DropDB Apollo11EasyTest

#Application Apollo11.Win
#Application Apollo11.Web

*FillForm
 User Name = Admin
*Action Log On

#IfDef Apollo11.Win

*Action About...
 
*Action OK

#EndIf

#IfDef Apollo11.Web
*Action Navigation(My Details)
#EndIf
