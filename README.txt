Wolvesbane Save Optimization - Phase 3: Token / OWLTR Read-Only Audit
=====================================================================

PURPOSE
This phase does NOT delete, merge, move, or alter any object.
It measures the live Daat99 token population and inspects the Daat99OWLTR
control object's collection sizes so we can design the next optimization safely.

INSTALL
Copy:
  Scripts/Custom/Wolvesbane Written/AI Written/World Cleanup/WBTokenAudit.cs
into the matching location under your TEST server Scripts folder.

Restart/recompile the TEST server.

COMMANDS
  [WBTokenAudit
      Basic read-only audit.

  [WBTokenAudit verbose
      Adds the largest immediate parents containing token objects and displays
      smaller OWLTR collection fields as well.

IMPORTANT
Please run the basic command first and send the complete output/screenshots.
Then run verbose and send those screenshots too.

Nothing is deleted or modified by this script.
