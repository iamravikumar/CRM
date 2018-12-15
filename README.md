# CRM
A real-time CRM web application that based on ASP.NET Core.

# Introduction

The application has been developing based on the real sector. In the application there exist many modules. Each module is to be manipulated by authorized users if the module is including any roles or authorize. Currently, the application consists of three roles such as user, member, officer. Those roles can be manipulated their modules. Moreover, the application is being splitted according to Team Module so each team member can be viewed own Team's datas (personnels, firms, schedules, etc.).

# Modules

- Team Module (can access from all roles),
- Team Member Module (can access from member and officer),
- Personnel Module (can access from member and officer),
- Firm Module (can access from member and officer),
- Sector Module (can only access from officer, also this module feeds Firm),
- Detailed Schedule Module (can access from member and officer),
- Programme Module (can only access from officer, also this module feeds Schedule),
- Product Module (can only access from officer, also this module feeds Service),
- Service Module (can only access from officer),
- Payment Module (can access from member and officer),
- Payment Options Module (can only access from officer, also this module feeds Service),
- Identity Module (can access from all roles),
