# epf-sudoku-MINA

TP sudoku du groupe 1.

Pas de groupe arbitre défini.

Les solvers SMT Z3, CSP et Norvig sont ajouté. Pour run le benchmark, ajouter les fichiers libz3.dylib et libz3.dll dans le dossier /bin/Debug/netcoreapp2.2 du projet Benchmark pour ajouter al dépendance manuellement.

Le Benchmark choisit un des trois fichiers de sudokus à résoudre et les affiches les uns en dessous des autres avec le temps de résolution de pour chaque solver de chaque sudoku. Egalement à la fin de la résolution le temps de résolution total du fichier est affiché.


Pour run le projet, il suffit de fork le projet et de le lancer via Visual Studio (ou directement en ligne de commande). Cependant, le projet tourne avec une version .NET Core 2.2 qui est donc nécessaire.
