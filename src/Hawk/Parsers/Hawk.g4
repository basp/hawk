grammar Hawk;

root        : def* pattern* ;

def         : ID ':' pattern ';' ;

pattern     : toklist
            | parens   
            | brackets
            | escaped
            ;

escaped     : '"' .*? '"' ;

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

NUM         : [1-9][0-9]* ;
ID          : [A-Z] ;
TEXT        : CHAR+ ;
WS          : [ \t\r\n] -> skip ;

fragment CHAR        
            : 'a'..'z'
            | '\''
            | [\u00C0-\uFEFC]
            ;