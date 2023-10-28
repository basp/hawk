grammar Hawk;

root        : sub* pattern+ ;

sub         : ID ':' pattern ';' ;

pattern     : toklist
            | pgroup   
            | bgroup
            | ESC
            ;

pgroup      : '(' pattern+ ')' filter* 
            ;
bgroup      : '[' pattern+ ']' filter* 
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
