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
    UniversityDto
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './edit-university-dialog.component.html'
})
export class EditUniversityDialogComponent extends AppComponentBase implements OnInit {
    saving = false;
    university = new UniversityDto();
    id: number;

    @Output() onSave = new EventEmitter<any>();

    constructor(
        injector: Injector,
        public _universityService: UniversityServiceProxy,
        public bsModalRef: BsModalRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._universityService.get(this.id).subscribe((result) => {
            this.university = result;
        });
    }

    save(): void {
        this.saving = true;

        this._universityService.update(this.university).subscribe(
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
