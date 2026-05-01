FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN apt-get update && apt-get install -y \
    git \
    curl \
    unzip \
    less \
    && rm -rf /var/lib/apt/lists/*

# Install CSharpier as a global .NET tool
RUN dotnet tool install -g csharpier
ENV PATH="$PATH:/root/.dotnet/tools"

# Git tools
RUN git config --global push.autoSetupRemote true
RUN curl -sSfL https://raw.githubusercontent.com/SimonNyvall/b-branch/main/install.sh | sh

# Enable DOD support (Docker Outside of Docker)
RUN apt-get update && apt-get install -y docker.io