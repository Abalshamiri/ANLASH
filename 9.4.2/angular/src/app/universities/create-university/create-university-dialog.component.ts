import {
    Component,
    Injector,
    OnInit,
    EventEmitter,
    Output
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityServiceProxy,
    CreateUniversityDto
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './create-university-dialog.component.html'
})
export class CreateUniversityDialogComponent extends AppComponentBase implements OnInit {
    saving = false;
    university = new CreateUniversityDto();

    @Output() onSave = new EventEmitter<any>();

    constructor(
        injector: Injector,
        public _universityService: UniversityServiceProxy,
        public bsModalRef: BsModalRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.university.isActive = true;
    }

    save(): void {
        this.saving = true;

        this._universityService.create(this.university).subscribe(
            () => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.bsModalRef.hide();
                this.onSave.emit();
            },
            () => {
                this.saving = false;
            }
        );
    }
}
