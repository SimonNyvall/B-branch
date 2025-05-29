# B-branch 1.0.a
- Branch information: Displays the branch name, the date of the last commit, the number of commits ahead or behind the upstream branch, and the branch description.
- Branch description: Git offers the ability to add a description to a branch. B-branch displays this description in the output.
- Pager interface: If the output is too large to fit on the screen, the output will be displayed in a pager interface.

# B-brnach 1.0.1
- Fix help message text to reflect actual useage.
- Fix pager interface issue when resizing window to fit all branches.
- Fix version flag prompt.
- Seach now captures more relevent information while in pager view.
- Pager view now indecates when the user have reached the end of the list.
- Fix short no-contains flag not parsing correctly.
- Fix search, no when on hit, the console will jump to fit search results.
- Time prefix not display 24h clock and 12h clock depending on system setting.
- Search do not highligh only white space.
- Fix branch description rendering in pager interface

# B-branch 1.1.0
- Add pager and --no-pager flags to force pager into the Console output.
- Pager now works with the --quite and -q flag
- Validation for -a, --all, -r, --remote, -q, --quite, -v, --version, --pager, --no-pager, -h, and --help that do not need an input.
- Add flag concatenation for short flags. Example git bb -qa.
- Last commit now also reflect time in months.
- Speed up performance for fetching git information

# B-branch 1.1.1
- Upgrade to .NET 9.
- Left paddring on `months ago` text fixed.
- Corrected calculations in months difference.
- Search highlight does not disable the current working branch color.

# B-branch 1.1.2
- Fix branch listing after git maintenance cleaning.
- Branches that does not have a date associated with them will now appear as -- instead of 5060 months ago

# B-branch 1.1.3
- Improved performance regarding branch aggregation.
- Fixed issue with branch duplications in the output.
- Fixed issue with `flag` priority when using multiple flags.