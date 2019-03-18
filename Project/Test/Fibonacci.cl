#Fibonacci;
Assign digit 20;
Assign lastNumber 1;
Assign lastLastnumber 1;

Print {lastLastnumber};
Print {lastNumber};

Zone fibb;

Operation {digit} - 1;
Update digit {Temporal};
Operation {lastNumber} + {lastLastnumber};
Update lastLastnumber {lastNumber};
Update lastNumber {Temporal};

Print {lastNumber};

If {{digit} <= 0} jumpto 1;
JumpTo fibb;

END;
