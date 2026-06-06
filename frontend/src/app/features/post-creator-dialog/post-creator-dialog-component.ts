import { Component, inject, signal } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiFileLike, TuiInputFiles, TuiAvatar, TuiFilesComponent, TuiFile, TuiBadge, TuiFiles, TuiInputChipComponent, TuiInputChipDirective, TuiTextareaComponent, TuiButtonCopy } from "@taiga-ui/kit";
import { TuiLink, TuiTextfieldComponent, TuiTextfieldMultiComponent, TuiInputDirective, TuiAppearance, TuiButton, TuiGroup, TuiDialogContext } from "@taiga-ui/core";
import { FilePreviewPipe } from "../../shared/pipes/file-preview-pipe";
import { POLYMORPHEUS_CONTEXT } from '@taiga-ui/polymorpheus';

@Component({
  selector: 'app-post-creator-dialog-component',
  imports: [TuiInputFiles, TuiAvatar, TuiLink, TuiFiles, FormsModule, TuiFile, FilePreviewPipe, TuiTextfieldComponent, TuiTextfieldMultiComponent, TuiInputChipComponent, TuiInputChipDirective, TuiInputDirective, TuiTextareaComponent, TuiButton, TuiAppearance, TuiGroup],
  templateUrl: './post-creator-dialog-component.html',
  styleUrl: './post-creator-dialog-component.scss',
})
export class PostCreatorDialogComponent {
  private readonly context = inject<TuiDialogContext<boolean, void>>(POLYMORPHEUS_CONTEXT);

  files = signal<File[]>([]);
  tags = signal<string[]>([]);

  onTagsChange(newValue: string[]): void {
    const validTags = newValue.filter((item) => item.startsWith('#') && item.length > 1);
    this.tags.set(validTags);
  }

  onCancel(): void {
    this.context.completeWith(false);
  }

  onCreate(): void{
    
  }
}
