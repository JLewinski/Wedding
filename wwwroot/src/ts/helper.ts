export class FormHelper {
    public static buildFormData(data: any) {
        const formData = new FormData();
        for (const key in data) {
            if (typeof (data[key]) == 'object' && !(data[key] instanceof Date) && !(data[key] instanceof File)) {
                this.buildFormDataRecursive(formData, data[key], key);
            } else {
                formData.append(key, data[key] as string);
            }
        }
        return formData;
    }

    private static buildFormDataRecursive(formData: FormData, data: any, rootKey: string) {
        for (const key in data) {
            if (typeof (data[key]) == 'object' && !(data[key] instanceof Date) && !(data[key] instanceof File)) {
                this.buildFormDataRecursive(formData, data[key], `${rootKey}.${key}`);
            } else {
                let fullKey = data instanceof Array ? `${rootKey}[${key}]` : `${rootKey}.${key}`;
                formData.append(fullKey, data[key] as string);
            }
        }
    }
}