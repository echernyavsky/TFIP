﻿module TFIP.Web.UI.Clients {
    
    export interface ICreateIndividualClientScope extends MasterPage.IMasterPageScope {
        clientViewModel: ClientViewModel;
        createUser: () => void;

        male: Gender;
        female: Gender;
        genders: Shared.ListItem[];
        countries: Shared.ListItem[];

        createClientForm: Core.ICustomFormController;
    }

    export class CreateIndividualClientController {
        public static $inject = [
            "$scope",
            "messageBox",
            "clientService",
            "$location",
            "locationHelperService",
            "urlBuilderService"
        ];

        constructor(
            private $scope: ICreateIndividualClientScope,
            private messageBox: Core.IMessageBoxService,
            private clientService: IClientService,
            private $location: ng.ILocationService,
            private locationHelperService: Core.LocationHelperService,
            private urlBuilderService: Core.IUrlBuilderService) {

            this.init();
        }

        private init() {
            var promsie = this.clientService.getClientFormViewModel().then((data: IndividualClientFormViewModel) => {
                this.$scope.countries = data.countries;
                this.$scope.clientViewModel = new ClientViewModel();

                

                this.$scope.$watch("clientViewModel",(newVal, oldVal) => {
                    for (var prop in this.$scope.clientViewModel) {

                        if (typeof (this.$scope.clientViewModel[prop]) == "string") {
                            this.$scope.clientViewModel[prop] = this.$scope.clientViewModel[prop].toUpperCase();
                        }
                    }
                }, true);
            });

            this.$scope.genders = [{ id: Gender.Male.toString(), value: "Мужской" }, { id: Gender.Female.toString(), value: "Женский" }];
            this.$scope.male = Gender.Male;
            this.$scope.female = Gender.Female;
            this.$scope.createUser = () => this.createUser();
        }

        private createUser() {
            if (this.$scope.createClientForm.$valid) {
                var promsie = this.clientService.createClient(this.$scope.clientViewModel);

                promsie.then((data: Shared.AjaxViewModel<any>) => {
                    if (data.isValid) {
                        this.locationHelperService.redirect(this.urlBuilderService.buildQuery("/Clients", { clientId: data.data.id, clientType: new ClientType().individualClient }));
                    } else {
                        this.messageBox.showErrorMulty(Const.Messages.clients, data.errors);
                    }
                }, (reason: any) => {
                        this.messageBox.showError(Const.Messages.clients, reason.message);
                });
            } else {
                //this.$scope.createClientForm.fieldInputForm.$setDirty();

                if (this.$scope.createClientForm.$error.required) {
                    for (var i = 0; i < this.$scope.createClientForm.$error.required.length; i++) {
                        if (this.$scope.createClientForm.$error.required[i].fieldInput) {
                            this.$scope.createClientForm.$error.required[i].fieldInput.$dirty = true;
                        }
                    }
                }
                if (this.$scope.createClientForm.$error.pattern) {
                    for (var i = 0; i < this.$scope.createClientForm.$error.pattern.length; i++) {
                        if (this.$scope.createClientForm.$error.pattern[i].fieldInput) {
                            this.$scope.createClientForm.$error.pattern[i].fieldInput.$dirty = true;
                        }
                    }
                }

                this.messageBox.showError(Const.Messages.clients, Const.Messages.invalidForm);
            }
        }
       
    }
}    