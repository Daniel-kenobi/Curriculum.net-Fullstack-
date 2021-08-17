import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CurriculumModel } from '../../models/curriculo.model';
import { cepService } from '../../services/cep.service';
import { curriculoService } from '../../services/curriculo.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  curriculoGroup: FormGroup;
  curriculo: CurriculumModel = new CurriculumModel();

  constructor(private frmBuilder: FormBuilder, private cep_service: cepService, private curriculo_service: curriculoService) {
    this.curriculoGroup = this.frmBuilder.group(
      {
        ID: 0,
        Nome: ['', [Validators.required, Validators.minLength(3)]],
        Email: ['', [Validators.email, Validators.required]],
        Telefone: ['', [Validators.minLength(10), Validators.maxLength(11), Validators.required]],
        Endereco: this.frmBuilder.group({
          cep: ['', [Validators.required, Validators.minLength(8)]],
          Logradouro: [''],
          Complemento: [''],
          Bairro: [''],
          Localidade: [''],
          UF: ['']
        }),
        FraseMotivacional: [''],
        Linkedin: [''],
        GitHub: [''],
        Instagram: [''],
        lst_infos_academicas: this.frmBuilder.array([]),
        lst_Historico_Profissional: this.frmBuilder.array([]),
        lst_soft_skills: this.frmBuilder.array([])
      })
  }

  fnc_adiciona_infos_academicas() {
    const infos = this.curriculoGroup.controls.lst_infos_academicas as FormArray;

    if (infos.length == 3)
      return;

    infos.push(this.frmBuilder.group(
      {
        Nome_instituicao: '',
        TipoCurso: '',
        Curso: '',
        Descricao_aprendizado: '',
        DataInicio: '',
        DataConclusao: ''
      }))
  }

  fnc_adiciona_Historico_Profissional() {
    const infos = this.curriculoGroup.controls.lst_Historico_Profissional as FormArray;

    if (infos.length == 3)
      return;

    infos.push(this.frmBuilder.group(
      {
        Nome_instituicao: [''],
        Cargo: [''],
        Descricao_cargo: [''],
        DataInicio: [''],
        DataSaida: [''],
      }))
  }

  fnc_adiciona_lst_soft_skills() {
    const infos = this.curriculoGroup.controls.lst_soft_skills as FormArray;

    if (infos.length == 3)
      return;

    infos.push(this.frmBuilder.group(
      {
        Nome: [''],
        Descricao: ['']
      }))
  }

  ngOnInit(): void {

  }

  fnc_cria_modelo(): CurriculumModel {
    const curriculo = new CurriculumModel();
    curriculo.ID = this.curriculoGroup.controls['ID'].value;
    curriculo.Nome = this.curriculoGroup.controls['Nome'].value;
    curriculo.Email = this.curriculoGroup.controls['Email'].value;
    curriculo.Telefone = this.curriculoGroup.controls['Telefone'].value;
    curriculo.Endereco = this.curriculoGroup.controls['Endereco'].value;
    curriculo.FraseMotivacional = this.curriculoGroup.controls['FraseMotivacional'].value;
    curriculo.Linkedin = this.curriculoGroup.controls['Linkedin'].value;
    curriculo.GitHub = this.curriculoGroup.controls['GitHub'].value;
    curriculo.Instagram = this.curriculoGroup.controls['Instagram'].value;
    curriculo.lst_infos_academicas = this.curriculoGroup.controls['lst_infos_academicas'].value;
    curriculo.lst_Historico_Profissional = this.curriculoGroup.controls['lst_Historico_Profissional'].value;
    curriculo.lst_soft_skills = this.curriculoGroup.controls['lst_soft_skills'].value;

    return curriculo;
  }    

  onSubmit() {
    const curriculo = this.fnc_cria_modelo();

    console.log(curriculo);

    this.curriculo_service.fnc_cria_curriculo(curriculo).subscribe(x => { console.log(x) }, (err) => { console.log(err) });
  }
}
