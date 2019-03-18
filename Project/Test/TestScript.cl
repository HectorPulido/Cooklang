#Esto es un comentario;
Print hola mundo que onda;
jumpto estoesunazona;
Print esto va a salir pero despues;
zone estoesunazona;
Print esto esta dentro de la zona y va a salir 2 veces;
return;
assign minombre2 Andres;
assign minombre Hector;
Print este texto contiene variables ejemplo: minombre {minombre};
jump  11;
Print esto no va a salir;
Print esto si;
update minombre {minombre2};
Print ahora la variable minombre cambio a {minombre};
jump +1;
print esto no va a salir por el +1;
print otra ves, esto si;

print <------aqui entran las matematicas------>;
Operation (7 + 21) * 33 ;
print {Temporal};

assign entrada1 {Temporal};
assign entrada2 25;
print {entrada1} / {entrada2};

Operation {entrada1} / {entrada2} ;
print {Temporal};

print <------aqui entran los condicionales------>;
if true jumpto 1;
print esto no va a salir;
print esto si;

print <------aqui entran los condicionales REALES------>;
if {{minombre}= Andres} jumpto 1;
print esto no va a salir;
print esto si;

assign x 20;

if {{x} < 15} jumpto 1;
print esto va a salir;
print esto si;

if {{{x} >= 15} and (not{{x} = 20}}) jumpto 1;
print 1;
print 2;

END