import React, { useMemo } from 'react';
import { Editor } from '@tinymce/tinymce-react';

interface WysiwygEditorProps {
    value: string;
    onChange: (content: string) => void;
    label?: string;
}

export const WysiwygEditor: React.FC<WysiwygEditorProps> = ({ value, onChange, label }) => {
    const editorConfig = useMemo(() => ({
        height: 400,
        menubar: false,
        plugins: [
            'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
            'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
            'insertdatetime', 'media', 'table', 'code', 'help', 'wordcount'
        ],
        toolbar: 'undo redo | blocks | ' +
            'bold italic forecolor | alignleft aligncenter ' +
            'alignright alignjustify | bullist numlist outdent indent | ' +
            'removeformat | help',
        content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
    }), []);

    return (
        <div className="mb-4">
            {label && <label className="block text-sm font-medium text-gray-700 mb-1">{label}</label>}
            <Editor
                apiKey="no-api-key"
                tinymceScriptSrc="https://cdnjs.cloudflare.com/ajax/libs/tinymce/6.8.3/tinymce.min.js"
                init={editorConfig}
                value={value}
                onEditorChange={(content) => onChange(content)}
            />
        </div>
    );
};
