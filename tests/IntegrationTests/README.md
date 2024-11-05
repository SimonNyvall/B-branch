To run the tests please check out the [Test Documentation](/docs/Tests.md).

> [!NOTE]
> These tests are not meant to be run on a regular basis, on a local machine.
> To run the tests, you need to have docker installed to the environment is isolated from the host machine.
> The container will setup a git repository with all the paramers for the test to be ran correctly.

### Structure of Integration Tests
The [IntegrationBase](./Base/IntegrationBase.cs) class helps the integration tests start and run processes of B-branch.