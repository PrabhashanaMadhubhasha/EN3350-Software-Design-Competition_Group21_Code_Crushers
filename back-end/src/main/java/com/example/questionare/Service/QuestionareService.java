package com.example.questionare.Service;

import com.example.questionare.DTO.AnswersDTO;
import com.example.questionare.DTO.QuestionDTO;
import com.example.questionare.DTO.QuestionFeedbackDTO;
import com.example.questionare.DTO.ResultDTO;
import com.example.questionare.Entity.AnswersEntity;
import com.example.questionare.Entity.FeedBackEntity;
import com.example.questionare.Entity.QuestionEntity;
import com.example.questionare.Repositery.AnswersRepo;
import com.example.questionare.Repositery.FeedbackRepo;
import com.example.questionare.Repositery.QuestionRepo;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class QuestionareService {
    @Autowired
    private QuestionRepo questionRepo;

    @Autowired
    private AnswersRepo answersRepo;

    @Autowired
    private FeedbackRepo feedbackRepo;

    @Autowired
    private ModelMapper modelMapper;

    private boolean isPlayed  = false;

    public QuestionDTO getQuestionByNumber(int qNumber) {
        QuestionEntity questionEntity = questionRepo.findQuestionByQuestionNum(qNumber);
        return modelMapper.map(questionEntity, QuestionDTO.class);
    }

    public void updateAnswer(int question_num, int userIndex, int userAnswer) {
        List<AnswersEntity> userAnswerList = answersRepo.findByUserIndexAndQuestionNum(userIndex, question_num);
        if (userAnswerList.isEmpty()){
            // If no answer exists for the given user index and question number, create a new answer entity
            AnswersEntity answersEntity = new AnswersEntity();
            answersEntity.setUserIndex(userIndex);
            answersEntity.setQuestion_num(question_num);
            answersEntity.setUserAnswer(userAnswer);
            answersRepo.save(answersEntity);
        } else {
            // If an answer exists for the given user index and question number, update the existing answer entity
            AnswersEntity existingAnswer = userAnswerList.get(0); // Since there is only one answer per user index and question number
            existingAnswer.setUserAnswer(userAnswer); // Update the user's answer
            answersRepo.save(existingAnswer); // Save the updated answer entity
        }
    }

    public void save_Questions_and_Feedbacks(List<QuestionFeedbackDTO> questionfeedbackDTOList) {
        try {
            questionRepo.deleteAll();
            feedbackRepo.deleteAll();
            for (QuestionFeedbackDTO questionFeedbackDTO : questionfeedbackDTOList) {
                QuestionEntity questionEntity = new QuestionEntity();
                questionEntity.setQuestion_num(questionFeedbackDTO.getQuestion_num());
                questionEntity.setQuestion(questionFeedbackDTO.getQuestion());
                questionEntity.setOption1(questionFeedbackDTO.getOption1());
                questionEntity.setOption2(questionFeedbackDTO.getOption2());
                questionEntity.setOption3(questionFeedbackDTO.getOption3());
                questionEntity.setOption4(questionFeedbackDTO.getOption4());
                questionRepo.save(questionEntity);

                FeedBackEntity feedBackEntity = new FeedBackEntity();
                feedBackEntity.setQuestion_num(questionFeedbackDTO.getQuestion_num());
                feedBackEntity.setCommonFeedback(questionFeedbackDTO.getCommonFeedback());
                feedBackEntity.setFeedback1(questionFeedbackDTO.getFeedback1());
                feedBackEntity.setFeedback2(questionFeedbackDTO.getFeedback2());
                feedBackEntity.setFeedback3(questionFeedbackDTO.getFeedback3());
                feedBackEntity.setFeedback4(questionFeedbackDTO.getFeedback4());
                feedBackEntity.setCorrect_answer(questionFeedbackDTO.getCorrectAnswer());
                feedbackRepo.save(feedBackEntity);
            }
        } catch (Exception e) {
            e.printStackTrace();
            throw new RuntimeException("Failed to save.", e);
        }
    }

    public List<ResultDTO> getResult(int userIndex) {
        isPlayed = true;
        List<QuestionEntity> questions = questionRepo.findAll();
        int length = questions.size();

        List<ResultDTO> results = new ArrayList<>(length);
        for (int qNum = 1; qNum <= length; qNum++) {
            ResultDTO resultDTO = new ResultDTO();
            // set question number on the result dto
            resultDTO.setQuestion_num(qNum);
            //set the question on the result dto
            QuestionEntity questionEntity = questionRepo.findQuestionByQuestionNum(qNum);
            QuestionDTO questionDTO = modelMapper.map(questionEntity, QuestionDTO.class);
            resultDTO.setQuestionDTO(questionDTO);
            // get the answer provided by the user
            List<AnswersEntity> answersEntityList = answersRepo.findByUserIndexAndQuestionNum(userIndex, qNum);
            AnswersEntity userAnswer = answersEntityList.get(0);

            FeedBackEntity feedBackEntity = feedbackRepo.findFeedbackByQuestionNum(qNum);
            // check weather answer is correct or not
            resultDTO.setStatus(userAnswer.getUserAnswer() == feedBackEntity.getCorrect_answer());

            resultDTO.setCorrectAns(feedBackEntity.getCorrect_answer());
            resultDTO.setCommonFeedback(feedBackEntity.getCommonFeedback());
            // set feedback based on the user's answer
            switch (userAnswer.getUserAnswer()) {
                case 1:
                    resultDTO.setSpecialFeedback(feedBackEntity.getFeedback1());
                    break;
                case 2:
                    resultDTO.setSpecialFeedback(feedBackEntity.getFeedback2());
                    break;
                case 3:
                    resultDTO.setSpecialFeedback(feedBackEntity.getFeedback3());
                    break;
                case 4:
                    resultDTO.setSpecialFeedback(feedBackEntity.getFeedback4());
                    break;

            }
            results.add(resultDTO);
        }
        return results;
    }

    public int numberOfQuestions(){
        List<QuestionEntity> questions = questionRepo.findAll();
        return questions.size();
    }

    public AnswersDTO getUserAnswer(int userIndex, int questionNum) {
        List<AnswersEntity> answersEntities = answersRepo.findByUserIndexAndQuestionNum(userIndex, questionNum);
        AnswersDTO answersDTO = new AnswersDTO();
        if (!answersEntities.isEmpty()) {
            AnswersEntity userAnswer = answersEntities.get(0);
            answersDTO.setAnswer(userAnswer.getUserAnswer());
            answersDTO.setUserIndex(userIndex);
            answersDTO.setQuestion_num(questionNum);
        } else {
            answersDTO.setAnswer(0);
            answersDTO.setUserIndex(userIndex);
            answersDTO.setQuestion_num(questionNum);
        }
        return answersDTO;
    }

    public Boolean getStatus(){
        return isPlayed;
    }

    // Get total marks of the player
    public int getUserMarks(int userIndex) {
        List<AnswersEntity> answersEntities = answersRepo.findByUserIndex(userIndex);
        int marks = 0;
        for (AnswersEntity answersEntity : answersEntities) {
            int question_number = answersEntity.getQuestion_num();
            int userAnswer = answersEntity.getUserAnswer();
            FeedBackEntity feedBackEntity = feedbackRepo.findFeedbackByQuestionNum(question_number);
            int correct_ans = feedBackEntity.getCorrect_answer();
            if (userAnswer == correct_ans) {
                marks = marks + 1;
            }
        }

        return marks;
    }

}
