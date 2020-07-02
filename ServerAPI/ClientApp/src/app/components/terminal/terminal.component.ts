import { Component, OnInit, AfterViewInit, ViewChild, Input } from '@angular/core';
import { NgTerminal } from 'ng-terminal';
import MachineService from 'src/app/services/machine.service';
import { TerminalType } from 'src/app/models/command.model';

@Component({
  selector: 'terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.css']
})
export class TerminalComponent implements OnInit, AfterViewInit {
  @ViewChild('term', { static: true }) child: NgTerminal;

  @Input() terminalType: TerminalType;
  @Input() machineIp: string;

  currentCommand: string = '';
  currentDirectory: string = '';

  constructor(private machineService: MachineService) { }

  ngOnInit() {
  }

  newLine(){
    this.child.write('\r\n'+ `${this.currentDirectory}> `);
  }

  async ngAfterViewInit(){
    let result = await this.machineService.sendCommand(this.machineIp,{
      text: 'cd',
      type: this.terminalType
    });
    this.currentDirectory = result.directory;

    this.newLine();

    this.child.keyEventInput.subscribe(async e => {
      const ev = e.domEvent;
      const printable = !ev.altKey && !ev.ctrlKey && !ev.metaKey;

      if (ev.keyCode === 13) {
        if(this.currentCommand){
          console.log(this.currentCommand)
          let result = await this.machineService.sendCommand(this.machineIp,{
            text: this.currentCommand,
            type: this.terminalType,
            directory: this.currentDirectory
          })
          console.log(result)
          this.currentCommand = '';
          this.currentDirectory = result.directory;
          if(result.output) this.child.write('\r\n'+result.output);
        }
        this.newLine();
      } else if (ev.keyCode === 8) {
        // Do not delete the prompt
        if(this.currentCommand.length > 0) this.currentCommand = this.currentCommand.substr(0,this.currentCommand.length-1);
        if (this.child.underlying.buffer.active.cursorX > 5) {
          this.child.write('\b \b');
        }
      } else if (printable) {
        this.child.write(e.key);
        this.currentCommand += e.key;
      }
    })
  }
}
