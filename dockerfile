FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["owl-and-elk-press-BE/owl-and-elk-press-BE.csproj", "owl-and-elk-press-BE/"]
RUN dotnet restore "owl-and-elk-press-BE/owl-and-elk-press-BE.csproj"

COPY . .
WORKDIR "/src/owl-and-elk-press-BE"

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:${PORT}

ENTRYPOINT ["dotnet", "owl-and-elk-press-BE.dll"]
