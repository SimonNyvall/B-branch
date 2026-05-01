FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN apt-get update && apt-get install -y \
    git \
    curl \
    unzip \
    && rm -rf /var/lib/apt/lists/*

# Install CSharpier as a global .NET tool
RUN dotnet tool install -g csharpier

ENV PATH="$PATH:/root/.dotnet/tools"