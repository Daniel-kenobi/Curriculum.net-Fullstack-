import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html'
})
export class PerfilComponent implements OnInit {
  usuario_logado: usuarioModel;
  perfilGroup: FormGroup;
  imagem: string = null;

  constructor(private authService: authService, private frmBuilder: FormBuilder) {
    this.authService.fnc_redireciona();
  }

  ngOnInit() {
    this.usuario_logado = this.authService.usrLogado;

    if (this.usuario_logado.img_perfil) {
      this.usuario_logado.img_perfil = ("data:image/png;base64," + this.usuario_logado.img_perfil);

      if (this.usuario_logado.img_perfil.length > 0)
        this.imagem = this.usuario_logado.img_perfil;
    }

    if (this.usuario_logado) {
      this.perfilGroup = this.frmBuilder.group({
        Nome: [this.usuario_logado.nome || ''],
        Email: [this.usuario_logado.email || ''],
        Telefone: [this.usuario_logado.telefone || ''],
        cep: ['']
      })
    }
  }
}
