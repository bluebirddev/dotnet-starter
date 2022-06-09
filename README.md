# dotnet-starter
This repo contains starter code for a dotnet api

## Prerequisites

- Docker/docker desktop
- Dotnet core 6 LTS
- Visual Studio 2022 or similar

## Setup Locally

1. Create the postgres database in docker by running the script called `runlocaldb.sh` in the root directory.
2. Populate database structure by running `migrate.cmd` in the src directory
3. Signup/Login and create a new api application on www.auth0.com and follow the applicable quick start instructions.
4. Open up solution in Visual Studio and run. The project should open up on a swagger page.

## Project Overview

This project is a movie api. You can list the available pre-seeded movies or add more. Currently the methods to view movies are not secured, but you need to specify a valid bearer token to access the create method.

### Adding new entities to the db

1. Add a new model
2. Add this model to `DotnetStarterContext`

![image](https://user-images.githubusercontent.com/4198983/172861265-84ad2b86-a818-416b-a027-0f5d6d6d2baf.png)

3. Run `addmigrations.cmd` in the src directory and specify a name for this migration
4. (Optional) Persist changes locally by running `migrate.cmd` in the src directory

### Future additions

- Dockerizing project
- Unit tests

