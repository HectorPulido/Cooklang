#Fibonacci;
assign digit 20;
assign lastNumber 1;
assign lastLastnumber 1;

print {lastLastnumber};
print {lastNumber};

zone fibb;

operation {digit} - 1;
assign digit {Temporal};
operation {lastNumber} + {lastLastnumber};
assign lastLastnumber {lastNumber};
assign lastNumber {Temporal};

print {lastNumber};

if {{digit} <= 0} jumpto 1;
jumpto fibb;

end;
