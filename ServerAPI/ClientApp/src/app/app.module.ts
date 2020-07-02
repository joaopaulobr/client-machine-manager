import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './pages/home/home.component';
import { MachineComponent } from './components/machine/machine.component';
import { TerminalComponent } from './components/terminal/terminal.component';
import MachineService from './services/machine.service';
import { NgTerminalModule } from 'ng-terminal';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    MachineComponent,
    TerminalComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgTerminalModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ])
  ],
  providers: [MachineService],
  bootstrap: [AppComponent]
})
export class AppModule { }
