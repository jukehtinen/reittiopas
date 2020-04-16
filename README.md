
### Reittiopas

Reittiopas-koodihaaste! Simppeli ASP.NET Core palvelu, joka hostaa Vue frontendiä (./Reitti.Web/ClientApp) ja tarjoaa APIn reittien hakemiseen.

Kaikki logiikka on oikeastaan ./Reitti.Web/Services/RouteService.cs filessä. Se parsii rettiopas.json filen ja ratkoo reittejä dijkstran algoritmilla.

### Running the app

With Docker
* `docker build -t reittiopas .`
* `docker run --env PORT=8080 -p 5000:8080 reittiopas`
* Open http://localhost:5000

Locally
* Requires [dotnet sdk](https://dotnet.microsoft.com/download) and [nodejs](https://nodejs.org/en/).
* Build with `dotnet publish -c Release -o output`.
* Start by `cd output` and `dotnet Reitti.Web.dll`.
* Open http://localhost:5000

### Development
* Run tests with `dotnet test`.
* Start frontend by running `npm run serve` in `./Reitti.Web/ClientApp`.
* Start backend by running solution in VS/VSCode or `dotnet run Reitti.Web.csproj`.
