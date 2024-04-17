package com.example.questionare.Repositery;


import com.example.questionare.Entity.AnswersEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface AnswersRepo extends JpaRepository<AnswersEntity, Integer> {
    List<AnswersEntity> findByUserIndex(int userIndex);
    @Query("SELECT a FROM AnswersEntity a WHERE a.userIndex = :userIndex AND a.question_num = :questionNum")
    List<AnswersEntity> findByUserIndexAndQuestionNum(@Param("userIndex") int userIndex, @Param("questionNum") int questionNum);

}
