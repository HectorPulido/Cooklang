#Factorial;
assign digit 10;
assign count 1;

zone fact;

operation {digit} * {count};
assign count {Temporal};

operation {digit} - 1;
assign digit {Temporal};

if {{digit} <= 0} jumpto 1;
jumpto fact;

print {count};

END;
