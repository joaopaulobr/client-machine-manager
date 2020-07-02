import { Component, OnInit, Input } from '@angular/core';
import Machine from 'src/app/models/machine.model';
import { TerminalType } from 'src/app/models/command.model';

@Component({
  selector: 'machine',
  templateUrl: './machine.component.html',
  styleUrls: ['./machine.component.css']
})
export class MachineComponent implements OnInit {
  @Input() machine: Machine;
  @Input() terminalType: TerminalType = TerminalType.Powershell;

  constructor() { }

  ngOnInit() {
  }
}
