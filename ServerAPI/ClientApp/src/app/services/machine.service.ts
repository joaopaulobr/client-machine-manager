import { Injectable } from "@angular/core";
import Machine from "../models/machine.model";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "src/environments/environment";
import Command from "../models/command.model";

@Injectable()
export default class MachineService{
    constructor(private http: HttpClient){
    }

    public async getAllMachines() : Promise<Array<Machine>> {
        let machines: Array<Machine>;

        machines = await this.http.get<Array<Machine>>(`${environment.api_url}/machine`).toPromise();

        return machines;
    }

    public async sendCommand(ip: string, command: Command) : Promise<Command>{
        return await this.http.post<Command>(`${environment.api_url}/machine/command/${ip}`,command).toPromise();
    }

    public getAllMachinesMock() : Array<Machine> {
        let machines: Array<Machine> = new Array<Machine>();
    
        machines.push({
          name: 'Máquina 1',
          ip: '1.1.1.1',
          antivirus: 'Antivirus 1',
          windows_version: 'Versão windows 1',
          dotnet_version: 'Versão dotnet 1',
          total_hd_size: 1000,
          free_hd_size: 400,
        });
    
        machines.push({
          name: 'Máquina 2',
          ip: '1.1.1.1',
          antivirus: 'Antivirus 1',
          windows_version: 'Versão windows 1',
          dotnet_version: 'Versão dotnet 1',
          total_hd_size: 1000,
          free_hd_size: 400,
        });
    
        machines.push({
          name: 'Máquina 3',
          ip: '1.1.1.1',
          antivirus: 'Antivirus 1',
          windows_version: 'Versão windows 1',
          dotnet_version: 'Versão dotnet 1',
          total_hd_size: 1000,
          free_hd_size: 400,
        });
    
        machines.push({
          name: 'Máquina 4',
          ip: '1.1.1.1',
          antivirus: 'Antivirus 1',
          windows_version: 'Versão windows 1',
          dotnet_version: 'Versão dotnet 1',
          total_hd_size: 1000,
          free_hd_size: 400,
        });
    
        machines.push({
          name: 'Máquina 5',
          ip: '1.1.1.1',
          antivirus: 'Antivirus 1',
          windows_version: 'Versão windows 1',
          dotnet_version: 'Versão dotnet 1',
          total_hd_size: 1000,
          free_hd_size: 400,
        });

        return machines;
    }
}