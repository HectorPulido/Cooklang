#Factorial;
Assign digit 10;
Assign count 1;

Zone fact;

Operation {digit} * {count};
Update count {Temporal};

Operation {digit} - 1;
Update digit {Temporal};

If {{digit} <= 0} jumpto 1;
JumpTo fact;

Print {count};

END;
