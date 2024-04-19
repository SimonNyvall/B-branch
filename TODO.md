- [ ] Make the flags only take one or two -. Now they can take n amount.
- [ ] Then printing the branches with the --all flag needs a rework. Now it just prints everything, but if there is too many branches it should only print the max length of the terminal. Same for the other flags I guess.
- [ ] See if I can remove the CliWrap package. Maybe use the process class from dotnet and make it async mysself with a wrapper.
- [ ] Update the readme
- [ ] Add a (-p ; --print-top) flag. To only print the top branches.
- [ ] Maybe add a cleaning feature for old branches. Like a (-c ; --clean) flag. That will remove all branches that are not in the remote.

Remove ref keywords from methods. The biggest downside is the dotnet sdk in the startup and the ref wont really help against that. It just is not clean.

See over the git Base class. Clean it up.

The print class is a bit messy. Clean that up.

The error handling does not work as intended. It should print all the errors it gathers. Not just the first one.
