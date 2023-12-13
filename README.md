# Outlook Calendar Integration App

Este repositorio contiene una api capaz de integrarse con el calendario de outlook a traves de microsoft graph. Utiliza ASP.NET 7 y esta integrado a swagger para su uso.  

## Como usar: 
1. La aplicacion usa device-code flow authentication, por lo que es necesario iniciar sesion llamando al endpoint /DeviceCode 
![image](https://github.com/juancc1001/OutlookCalendarApiIntegration/assets/52534704/00b3094b-bd53-4938-bb8a-74808611cb23)
Responde un mensaje como el siguiente: 
  "To sign in, use a web browser to open the page https://microsoft.com/devicelogin and enter the code FJXBVGER9 to authenticate."

2. Obtener el access token llamando al endpoint /AccessToken 
![image](https://github.com/juancc1001/OutlookCalendarApiIntegration/assets/52534704/9ffa474e-ed4d-4724-8b90-6e736fd5fb58)
No es necesario guardar el token, queda guardado en la sesion de usuario (importante: la sesion expira en 5 minutos)

3. Ya se pueden utilizar los endpoints 
![image](https://github.com/juancc1001/OutlookCalendarApiIntegration/assets/52534704/5e5e5cbc-a196-4833-a409-8694a6db5e25)
