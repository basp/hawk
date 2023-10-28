grammar Hawk;

root        : def* pattern* ;

def         : ID ':' pattern ';' ;

pattern     : toklist
            | parens   
            | brackets
            | ESC
            ;

parens      : '(' pattern+ ')' filter* 
            ;
brackets    : '[' pattern+ ']' filter* 
            ;
toklist     : tok ('/' tok)* 
            ;
tok         : (TEXT | ID) ('*' NUM)? filter* 
            ;
filter      : '^' TEXT 
            ;

ESC         : '"' .*? '"' ;
NUM         : [1-9][0-9]* ;
ID          : [A-Z] ;
TEXT        : [a-z][a-z]* ;
WS          : [ \t\r\n] -> skip ;
