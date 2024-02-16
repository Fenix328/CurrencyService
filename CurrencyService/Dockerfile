# Вказуємо образ на основі якого буде працювати наш додаток 
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# Вказуємо робочу папку всередині контейнеру
WORKDIR /app
EXPOSE 80

# Вказуємо образ на основі якого будемо робити наш контейнер та відбуватись збірка додатка 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# Вказуємо робочу папку
WORKDIR /src
# Копіюємо файл CurrencyService.csproj
COPY ["CurrencyService.csproj", "CurrencyService.csproj"]
# Запускаємо restore для завантаження залежностей 
RUN dotnet restore "CurrencyService.csproj"
# Копіюємо усі файли із проекту в файлову систему контейнера
COPY . .
# Вказуємо робочу папку
WORKDIR /src
# Запускаємо збірку проекту в конфігурації Release з вихідною папкою 
RUN dotnet build "CurrencyService.csproj" -c Release -o /app/build

# Вказуємо образ який будемо використовувати для публікації
FROM build AS publish
# Запускаємо публікацію проекту в конфігурації Release з вихідною папкою 
RUN dotnet publish "CurrencyService.csproj" -c Release -o /app/publish

# Вказуємо образ на основі якого буде працювати наш додаток
FROM base AS final
# Вказуємо робочу папку всередині контейнеру
WORKDIR /app
# Копіюємо файли з образу publish в робочу папку
COPY --from=publish /app/publish .
# Вказуємо команду, яка буде запускати додаток
ENTRYPOINT ["dotnet", "CurrencyService.dll"]
