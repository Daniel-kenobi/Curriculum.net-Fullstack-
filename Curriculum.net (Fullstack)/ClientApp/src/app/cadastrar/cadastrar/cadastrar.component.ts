import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { cadastroModel } from '../../models/cadastro.model';

@Component({
  selector: 'app-cadastrar',
  templateUrl: './cadastrar.component.html',
})
export class CadastrarComponent implements OnInit {
  cadastrarForm: FormGroup;

  constructor(private frmBuilder: FormBuilder) { }

  ngOnInit() {
    this.cadastrarForm = this.frmBuilder.group({
      Nome: ['', [Validators.required, Validators.minLength(3)]],
      Email: ['', [Validators.required, Validators.email]],
      Senha: ['', Validators.required, Validators.minLength(6)],
      ConfirmacaoSenha: ['', Validators.required, Validators.minLength(6)]
    })
  }

  fnc_monta_modelo(): cadastroModel {
    const cadastro = new cadastroModel();
    cadastro.Nome = this.cadastrarForm.controls['Nome'].value;
    cadastro.Email = this.cadastrarForm.controls['Email'].value;
    cadastro.Senha = this.cadastrarForm.controls['Senha'].value;
    cadastro.ConfirmacaoSenha = this.cadastrarForm.controls['ConfirmacaoSenha'].value;

    return cadastro;
  }

  fnc_cadastra() {
    //this.cadastrarForm.controls['Nome'].setValue("Daniel....");
    console.log("concluido")
  }

}
