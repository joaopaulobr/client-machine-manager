import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import Machine from '../../models/machine.model';
import MachineService from 'src/app/services/machine.service';
import { TerminalType } from 'src/app/models/command.model';
import { NgTerminal } from 'ng-terminal';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{  
  public machines: Array<Machine>;

  constructor(private machineService: MachineService){
  }

  async ngOnInit() {
    this.machines = await this.machineService.getAllMachines();
  }
}
