import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { authService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html'
})
export class NavbarComponent implements OnInit {

  nome: string;

  constructor(private router: Router, private authService: authService) {
    this.fnc_forca_atualziacao_usuario();
  }

  ngOnInit() {

  }

  fnc_forca_atualziacao_usuario() {
    if (this.authService.fnc_retorna_usuario_logado())
      this.nome = this.authService.fnc_retorna_usuario_logado().Nome;
  }

}
