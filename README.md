if run/debug configurations aren't available, change this line

![image](https://github.com/user-attachments/assets/b95727e7-f0d4-444e-9173-c148fbbe98e8)

in 
.idea/idea.PrintMe/.idea/workspace.xaml

with your path to that file.

Client stack - TypeScript, Vite, React.
Pay attention to async/await and modules on ts/js.
Client dependencies - Axios for http requests (maybe others if you wish)
I recommend bootstrap for css styles and google api for map.

Server stack - Net 8, Asp.Net Core WebApi, C# 12.
Server dependencies - Entity framework (lastest) with Postgre, SignalR for chat logic.
Architecture:
![printmelogic](https://github.com/user-attachments/assets/60cc2e89-8f81-43ba-9e4b-2a64c6590e66)

Logic is set of rules according which models will behave.

Models is our entities we use in program. Models can encapsulate some simple logic. Complex logic should be moved to logic layer.

Persistence is database.

I think this architecture will simplify our development workflow because every non-trivial problem is a set of trivial ones and by giving structure to the solution of the problem we can't get confused in the solution and will concentrate on important things.

General naming conventions (more on msdn):
![image](https://github.com/user-attachments/assets/6c118d2f-5281-44e6-8b96-45eb28cd40c4)

