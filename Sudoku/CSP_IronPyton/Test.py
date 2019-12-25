
from timeit import default_timer as timer
from sudokucsp import SudokuCSP
from csp import backtracking_search, mrv, unordered_domain_values, forward_checking, mac, no_inference
#import numpy as np


class Test:
   

    def __init__(self):
        # empty board
        self.original_board = [[0 for j in range(9)] for i in range(9)]

    def set_board(self, level, which,grid):
        if level == 1:
            if which == 0:
                for i in range(9):
                    self.original_board[i] = grid[i]
              
                          
          

    def start(self, inf):
        s = SudokuCSP(self.original_board)
        
        self.start = timer()
        a = backtracking_search(s, select_unassigned_variable=mrv, order_domain_values=unordered_domain_values,
                                inference=inf)
        self.end = timer()
        if a:
            if(inf==mac):
                solution="mac"
            if(inf==forward_checking):
                solution="forward_checking"
            if(inf==no_inference):
                solution="no_inference"
            #print("\nSolution found by "+solution)
            
           # displaySudoku(a)
        else:
            
            print("\n Please check the sudoku initial board, solution not found!")
        self.bt = s.n_bt

    def display(self):
        time = round(self.end - self.start, 5)
        print("Time: " + str(time) + " seconds")
        print("N. BT: " + str(self.bt))

def parse_file(filename):
    return open(filename,"r").read().strip().split('\n')

def split_grid(grid): 
    return [char for char in grid]
        

def fill_grid(grid):
     pretable=split_grid(grid)
     #print(len(pretable))
     iterator=iter(pretable)
     
    
     def verify(str):
         if str==".":
             return 0
         else:
             return int(str)
     table=[[verify(iterator.next()) for y in range(9)] for x in range(9)]


     #displaySudokuNoResolved(table)

     return table

def displaySudoku(a):
    separedLine="-------------------------------------------------"
    blankedLine="        "
    line=""
    print(separedLine)
   
    for row in range(9):
                line="| "
                for column in range(9):
                        name = row * 9 + column
                        line+=" " + str(a["CELL"+str(name)]) + " "
                        
                        column=column+1
                        if(column != 0 | column % 3 == 0):
                            line+=" | "                       
                        else:
                            line+="  "                         
                        column=column-1
                print(line)                
                row=row+1
                if(row % 3 == 0):
                    line=separedLine
                    print(line)                                      
                row=row-1
                

def displaySudokuNoResolved(a):
    separedLine="-------------------------------------------------"
    blankedLine="        "
    line=""
    print(separedLine)
   
    for row in range(9):
                line="| "
                for column in range(0,9):
                        name = row * 9 + column
                        line+=" " +str(a[row][column]) + " "
                        
                        column=column+1
                        if(column != 0 | column % 3 == 0):
                            line+=" | "                       
                        else:
                            line+="  "                         
                        column=column-1
                print(line)                
                row=row+1
                if(row % 3 == 0):
                    line=separedLine
                    print(line)                                      
                row=row-1


def main(file,i):
    time = []
    back_track = []

    grids=parse_file(file)
   
    # modify inf to the type of inference that you want to do after assign a value
    # options are no_inference, forward_checking and mac
    #inf = mac
    # level 1 means level Easy and level 2 means level hard
    level = 1
    # which can assume 3 values: 0, 1 and 2 so we can use 3 boards for each level
    which = 0
    # if needed just modify self.which = self.which%3 to %4 in method set_board of Test Class
    # to add a new board, and add a clause "elif self.which == 3:
    #                                                             self.original_board[0] = first row
    #                                                             ...
    # 
    #                                                             self.original_board[8] = last row
    table=fill_grid(grids[i])
    
    inf = forward_checking    
    t1 = Test()
    t1.set_board(level, which,table)
    t1.start(inf)
    back_track.append(t1.bt)
    time.append(round(t1.end - t1.start, 5))
        
   
    
    



if __name__ == '__main__':
    main()
    input()

