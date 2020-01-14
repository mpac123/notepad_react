import React, { useState, useEffect, useRef } from "react";
import { apiService } from "./services";
import {
  Card,
  DatePicker,
  Pagination,
  Form,
  Input,
  Select,
  Checkbox,
  notification,
  message
} from "antd";
import {
  Loader,
  Button,
  Segment,
  Header,
  Icon,
  Input as SemanticInput
} from "semantic-ui-react";
import moment from "moment";

const { Option } = Select;

function hasErrors(fieldsError) {
  return Object.keys(fieldsError).some(field => fieldsError[field]);
}

const formItemLayout = {
  labelCol: {
    xs: { span: 24 },
    sm: { span: 8 }
  },
  wrapperCol: {
    xs: { span: 24 },
    sm: { span: 16 }
  }
};

const NoteEditForm = ({
  // form props
  form,

  // props
  selectedNoteID,
  setListMode
}) => {
  const [categories, setCategories] = useState([]);
  const [selectedNote, setSelectedNote] = useState(undefined);
  const semanticRef = useRef(null);

  useEffect(() => {
    const get = async () => {
      console.log("note id", selectedNoteID)
      if (selectedNoteID) {
        setSelectedNote(await apiService.getNote(selectedNoteID));
      }
      setCategories(await apiService.getCategories());
    };
    get();
  }, []);

  const addCategory = () => {
    const currentValue = semanticRef.current.inputRef.current.value;
    if (currentValue) {
      if (!categories.find(c => c.title === currentValue)) {
        setCategories([...categories, {title: currentValue}]);
      }
      semanticRef.current.inputRef.current.value = "";
    }
  };

  const handleSubmit = e => {
    e.preventDefault();
    console.log("Submitting!");
    form.validateFields(async (errs, values) => {
      
      if (errs) {
        notification.error({ message: "Validation errors occured!" });
      } else {
        values = {
          ...values,
          noteDate: moment(values.noteDate).format("YYYY-MM-DD"),
          timestamp: selectedNote ? selectedNote.timestamp : null
        }
        console.log("values", values)
        if (selectedNote) {
          
          try {
            await apiService.updateNote(selectedNote.noteID, values);
            message.success("The note has been updated");
          } catch {
            message.error("The update was not succesful");
            return
          }
        } else {
          try{
            await apiService.addNote(values);
            message.success("The note has been added");
          } catch {
            message.error("The note with such a title aready exists.")
            return
          }
        }
        setListMode();
      }
    });
  };

  const {
    getFieldDecorator,
    getFieldsError,
    getFieldError,
    isFieldTouched
  } = form;

  console.log("note", selectedNote);
  return (
    <Segment color="blue">
      <Header>
        <Icon name="edit" />{" "}
        {!selectedNote ? <> Create new </> : <>Update existing</>} note
      </Header>
      <Segment basic>
        <Form {...formItemLayout} onSubmit={handleSubmit}>
          <Form.Item label="Title">
            {getFieldDecorator("title", {
              rules: [{ required: true, message: "Please input the title!" }],
              initialValue: selectedNote ? selectedNote.title : undefined
            })(<Input placeholder="Title" />)}
          </Form.Item>
          <Form.Item label="Date">
            {getFieldDecorator("noteDate", {
              rules: [{ required: true, message: "Please input the date!" }],
              initialValue: selectedNote
                ? moment(selectedNote.noteDate)
                : undefined
            })(<DatePicker placeholder="yyyy-mm-dd" />)}
          </Form.Item>
          <Form.Item label="Is Markdown">
            {getFieldDecorator("isMarkdown", {
              valuePropName: "checked",
              initialValue: selectedNote ? selectedNote.isMarkdown : false
            })(<Checkbox>Is Markdown</Checkbox>)}
          </Form.Item>
          <Form.Item label="Content">
            {getFieldDecorator("description", {
              initialValue: selectedNote ? selectedNote.description : undefined
            })(<Input.TextArea placeholder="Content" rows="10" />)}
          </Form.Item>
          <Form.Item label="Categories">
            {getFieldDecorator("categories", {
              rules: [
                {
                  required: true,
                  message: "Please select your categories!",
                  type: "array"
                }
              ],
              initialValue: selectedNote ? selectedNote.categories : undefined
            })(
              <Select
                mode="multiple"
                placeholder="Please select your categories"
              >
                {categories.map(c => (
                  <Option key={c.title} value={c.title}>
                    {c.title}
                  </Option>
                ))}
              </Select>
            )}
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              disabled={hasErrors(getFieldsError())}
            >
              {selectedNote ? <span>Update</span> : <span>Create</span>}
            </Button>
          </Form.Item>
        </Form>
      </Segment>
      <hr />
      <Segment basic>
        <Header>Add category</Header>
        <SemanticInput ref={semanticRef} />
        <Button onClick={addCategory}>Add</Button>
      </Segment>
    </Segment>
  );
};

const NoteEdit = Form.create({ name: "note_edit" })(NoteEditForm);

export default NoteEdit;
